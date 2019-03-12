package sk.cll.mewsapp.activities;

import android.content.Context;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.os.Bundle;
import android.os.Handler;
import android.util.Log;
import android.view.View;
import android.widget.Toast;

import java.util.ArrayList;
import java.util.List;

import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.Toolbar;
import androidx.lifecycle.ViewModelProviders;
import androidx.recyclerview.widget.RecyclerView;
import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.schedulers.Schedulers;
import okhttp3.OkHttpClient;
import retrofit2.Retrofit;
import retrofit2.adapter.rxjava2.RxJava2CallAdapterFactory;
import retrofit2.converter.gson.GsonConverterFactory;
import sk.cll.mewsapp.R;
import sk.cll.mewsapp.data.Photo;
import sk.cll.mewsapp.data.utils.MyAdapter;
import sk.cll.mewsapp.data.utils.MyViewModel;
import sk.cll.mewsapp.data.utils.PaginationScrollListener;
import sk.cll.mewsapp.api.PhotoService;
import sk.cll.mewsapp.api.Repository;


public class ItemListActivity extends AppCompatActivity {

    public static final String BASE_URL = "http://jsonplaceholder.typicode.com/";
    public static final int LIMIT = 30;

    private boolean isLoading = false;
    private List<Photo> mPhotos;
    private CompositeDisposable mCompositeDisposable;
    private Repository mRepository;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_item_list);

        Toolbar toolbar = findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);
        toolbar.setTitle(getTitle());

        mRepository = new Repository(new Retrofit.Builder()
                .baseUrl(BASE_URL)
                .addConverterFactory(GsonConverterFactory.create())
                .client(new OkHttpClient.Builder().build())
                .addConverterFactory(GsonConverterFactory.create())
                .addCallAdapterFactory(RxJava2CallAdapterFactory.create())
                .build().create(PhotoService.class));

        mPhotos = new ArrayList<>();
        RecyclerView recyclerView = findViewById(R.id.item_list);
        mCompositeDisposable = new CompositeDisposable();
        setList();
        if (mPhotos.isEmpty()) {
            recyclerView.setAdapter(new MyAdapter(mPhotos, this));
            if (savedInstanceState == null) {
                downloadNewData();
            } else {
                findViewById(R.id.tv_empty_view).setVisibility(View.VISIBLE);
                View v = findViewById(R.id.tv_select);
                if (v != null) {
                    v.setVisibility(View.GONE);
                }
            }
        }

        recyclerView.addOnScrollListener(new PaginationScrollListener() {
            @Override
            protected void loadMoreItems() {
                isLoading = true;
                downloadNewData();
            }

            @Override
            public boolean isLoading() {
                return isLoading;
            }
        });
        findViewById(R.id.tv_empty_view).setOnClickListener(v -> downloadNewData());
    }

    /**
     * Sets isLoading to false after 1 second, so the download is not invoked many times while scrolling
     */
    private void setLoadingFalse() {
        new Handler().postDelayed(() -> isLoading = false, 1000);
    }

    public void downloadNewData() {
        if (isLoading) {
            return;
        }
        isLoading = true;
        Toast.makeText(ItemListActivity.this, R.string.downloading, Toast.LENGTH_SHORT).show();
        if (!checkInternetConnection(this)) {
            Toast.makeText(ItemListActivity.this, R.string.no_internet, Toast.LENGTH_LONG).show();
            setLoadingFalse();
            if (mPhotos.isEmpty()) {
                findViewById(R.id.tv_empty_view).setVisibility(View.VISIBLE);
                View v = findViewById(R.id.tv_select);
                if (v != null) {
                    v.setVisibility(View.GONE);
                }
            }
        } else {
            mCompositeDisposable.add(mRepository.executePhotoApi(mPhotos.size())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribeOn(Schedulers.io())
                    .subscribe(this::onSuccess, this::onError));
        }
    }

    public void onSuccess(List<Photo> photos) {
        findViewById(R.id.tv_empty_view).setVisibility(View.GONE);
        View v = findViewById(R.id.tv_select);
        if (v != null) {
            v.setVisibility(View.VISIBLE);
        }

        RecyclerView recyclerView = findViewById(R.id.item_list);
        assert recyclerView != null;

        mPhotos.addAll(photos);
        recyclerView.getAdapter().notifyDataSetChanged();
        setLoadingFalse();

        MyViewModel model = ViewModelProviders.of(this).get(MyViewModel.class);
        model.addPhotos(photos);
    }

    /**
     * Tries to load data from ViewModel
     */
    public void setList() {
        MyViewModel model = ViewModelProviders.of(this).get(MyViewModel.class);
        List<Photo> photos = model.getPhotos();
        if (photos != null) {
            mPhotos.addAll(photos);
            RecyclerView recyclerView = findViewById(R.id.item_list);
            recyclerView.setAdapter(new MyAdapter(mPhotos, this));
            recyclerView.getAdapter().notifyDataSetChanged();
        }
    }

    public void onError(Throwable error) {
        if (mPhotos.isEmpty()) {
            findViewById(R.id.tv_empty_view).setVisibility(View.VISIBLE);
            View v = findViewById(R.id.tv_select);
            if (v != null) {
                v.setVisibility(View.GONE);
            }
        }
        setLoadingFalse();

        Toast.makeText(ItemListActivity.this, R.string.error_downloading, Toast.LENGTH_LONG).show();
        Log.e("ItemList", error.getLocalizedMessage());
    }

    public static boolean checkInternetConnection(Context context) {
        ConnectivityManager connectivity = (ConnectivityManager) context
                .getSystemService(Context.CONNECTIVITY_SERVICE);
        if (connectivity == null) {
            return false;
        } else {
            NetworkInfo[] info = connectivity.getAllNetworkInfo();
            if (info != null) {
                for (NetworkInfo anInfo : info) {
                    if (anInfo.getState() == NetworkInfo.State.CONNECTED) {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
