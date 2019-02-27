package com.mews.task.net

import androidx.paging.DataSource
import androidx.paging.PositionalDataSource
import com.mews.task.data.*
import io.reactivex.Single
import io.reactivex.disposables.CompositeDisposable
import io.reactivex.disposables.Disposable
import io.reactivex.rxkotlin.subscribeBy
import io.reactivex.schedulers.Schedulers
import okhttp3.OkHttpClient
import okhttp3.logging.HttpLoggingInterceptor
import retrofit2.Call
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import retrofit2.http.GET
import retrofit2.http.Query
import java.io.IOException
import javax.inject.Singleton

@Singleton
class RetrofitItemDataSourceFactory(
    private val baseUrl: String,
    private val networkStateModel: NetworkStateModel
) : ItemDataSourceFactory() {

    override fun create(): DataSource<Int, Item> {
        return RetrofitItemDataSource.create(baseUrl, networkStateModel)
    }
}

private class RetrofitItemDataSource(
    private val itemService: ItemService,
    private val networkStateModel: NetworkStateModel
) : PositionalDataSource<Item>(), Disposable {

    private val compositeDisposable = CompositeDisposable()

    override fun isDisposed(): Boolean {
        return compositeDisposable.isDisposed
    }

    override fun dispose() {
        compositeDisposable.dispose()
    }

    override fun invalidate() {
        super.invalidate()
        dispose()
    }

    override fun loadInitial(params: LoadInitialParams, callback: LoadInitialCallback<Item>) {
        val startPosition = getStartPosition(params.requestedStartPosition)

        load(
            onCall = {
                networkStateModel.postState(NetworkLoading)
                itemService.listRepos(startPosition, params.requestedLoadSize)
            },
            onSuccess = { items ->
                networkStateModel.postState(NetworkSuccess)
                callback.onResult(items, startPosition)
            },
            onError = { throwable ->
                networkStateModel.postState(NetworkError(throwable))
            }
        )
    }

    override fun loadRange(params: LoadRangeParams, callback: LoadRangeCallback<Item>) {
        load(
            onCall = {
                networkStateModel.postState(NetworkLoading)
                itemService.listRepos(params.startPosition, params.loadSize)
            },
            onSuccess = { items ->
                networkStateModel.postState(NetworkSuccess)
                callback.onResult(items)
            },
            onError = { throwable ->
                networkStateModel.postState(NetworkError(throwable))
            }
        )
    }

    private fun getStartPosition(requestedStartPosition: Int): Int {
        return if (requestedStartPosition < 0) {
            0
        } else {
            requestedStartPosition
        }
    }

    private fun load(
        onCall: () -> Call<List<ItemDTO>>,
        onSuccess: (List<Item>) -> Unit,
        onError: (Throwable) -> Unit
    ) {
        Single.fromCallable { onCall().execute() }
            .retry(RETRY_COUNT)
            .subscribeOn(Schedulers.io())
            .subscribeBy(
                onSuccess = { response ->
                    if (response.isSuccessful) {
                        val dtoItems = response.body() ?: emptyList()
                        val domainItems = dtoItems.map { it.toDomain() }
                        onSuccess(domainItems)

                    } else {
                        onError(IOException("Response was not successful."))
                    }
                },
                onError = { throwable ->
                    onError(throwable)
                }
            ).also { disposable ->
                compositeDisposable.add(disposable)
            }
    }

    companion object {
        private const val RETRY_COUNT = 2L

        fun create(baseUrl: String, networkStateModel: NetworkStateModel): RetrofitItemDataSource {
            val logging = HttpLoggingInterceptor().also {
                it.level = HttpLoggingInterceptor.Level.HEADERS
            }

            val client = OkHttpClient.Builder()
                .addInterceptor(logging)
                .build()

            val retrofit = Retrofit.Builder()
                .client(client)
                .baseUrl(baseUrl)
                .addConverterFactory(GsonConverterFactory.create())
                .build()

            val service = retrofit.create(ItemService::class.java)
            return RetrofitItemDataSource(service, networkStateModel)
        }
    }
}

private interface ItemService {

    @GET("photos")
    fun listRepos(
        @Query("_start") start: Int,
        @Query("_limit") limit: Int
    ): Call<List<ItemDTO>>
}