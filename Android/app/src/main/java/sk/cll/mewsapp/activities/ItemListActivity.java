package sk.cll.mewsapp.activities;

import android.content.Context;
import android.content.res.Configuration;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.os.Bundle;
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
import sk.cll.mewsapp.paging.ItemDetailActivity;
import sk.cll.mewsapp.paging.Urls;
import sk.cll.mewsapp.services.PhotoService;

/**
 * An activity representing a list of Items. This activity
 * has different presentations for handset and tablet-size devices. On
 * handsets, the activity presents a list of items, which when touched,
 * lead to a {@link ItemDetailActivity} representing
 * item details. On tablets, the activity presents the list of items and
 * item details side-by-side using two vertical panes.
 */
public class ItemListActivity extends AppCompatActivity {

    public static final String BASE_URL = "http://jsonplaceholder.typicode.com/";
    public static final int LIMIT = 30;
    public static final int PAGE_START = 0;
    private int currentPage = PAGE_START;
    private boolean isLoading = false;
    int itemCount = 0;
    /**
     * Whether or not the activity is in two-pane mode, i.e. running on a tablet
     * device.
     */
    private boolean mTwoPane;
    private List<Photo> mPhotos;
    private CompositeDisposable mCompositeDisposable;
    PhotoService service = new Retrofit.Builder()
            .baseUrl(Urls.BASE_URL)
            .addConverterFactory(GsonConverterFactory.create())
            .client(new OkHttpClient.Builder().build())
            .addConverterFactory(GsonConverterFactory.create())
            .addCallAdapterFactory(RxJava2CallAdapterFactory.create())
            .build().create(PhotoService.class);

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_item_list);

        Toolbar toolbar = findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);
        toolbar.setTitle(getTitle());

        mTwoPane = getResources().getConfiguration().orientation == Configuration.ORIENTATION_LANDSCAPE;
        mPhotos = new ArrayList<>();
        RecyclerView recyclerView = findViewById(R.id.item_list);
        mCompositeDisposable = new CompositeDisposable();
        setList();
        if (mPhotos.isEmpty()) {
            recyclerView.setAdapter(new MyAdapter(mPhotos, this));
            downloadNewData();
        }

        recyclerView.addOnScrollListener(new PaginationScrollListener() {
            @Override
            protected void loadMoreItems() {
                isLoading = true;
                currentPage++;
                preparedListItem();

            }

            @Override
            public boolean isLoading() {
                return isLoading;
            }
        });

        findViewById(R.id.tv_empty_view).setOnClickListener(v -> {
           downloadNewData();
        });

    }

    public void downloadNewData() {
        if (!checkInternetConnection(this)) {
            Toast.makeText(ItemListActivity.this, R.string.no_internet, Toast.LENGTH_LONG).show();
            findViewById(R.id.tv_empty_view).setVisibility(View.VISIBLE);
        } else {
            mCompositeDisposable.add(service.getPhotos(mPhotos.size(), LIMIT)
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribeOn(Schedulers.io())
                    .subscribe(this::onSuccess, this::onError));
        }
    }

    public void onSuccess(List<Photo> photos) {
        findViewById(R.id.tv_empty_view).setVisibility(View.GONE);

        RecyclerView recyclerView = findViewById(R.id.item_list);
        assert recyclerView != null;
//                        setupRecyclerView((RecyclerView) recyclerView);


        mPhotos.addAll(photos);
//        recyclerView.setAdapter(new MyAdapter(mPhotos, this, mTwoPane));
        recyclerView.getAdapter().notifyDataSetChanged();
        isLoading = false;


        MyViewModel model = ViewModelProviders.of(this).get(MyViewModel.class);
        model.addPhotos(photos);


    }

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

    private void preparedListItem() {
        Toast.makeText(ItemListActivity.this, R.string.downloading, Toast.LENGTH_SHORT).show();
        downloadNewData();
    }

    public void onError(Throwable error) {
        findViewById(R.id.tv_empty_view).setVisibility(View.VISIBLE);

        Toast.makeText(ItemListActivity.this, R.string.error_downloading, Toast.LENGTH_SHORT).show();
        Log.e("ItemList", error.getLocalizedMessage());
    }

    //todo
    @Override
    protected void onDestroy() {
        super.onDestroy();
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
