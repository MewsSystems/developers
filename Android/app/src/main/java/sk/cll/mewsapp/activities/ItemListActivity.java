package sk.cll.mewsapp.activities;

import android.content.res.Configuration;
import android.os.Bundle;
import android.widget.Toast;

import java.util.ArrayList;
import java.util.List;

import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.Toolbar;
import androidx.lifecycle.ViewModelProviders;
import androidx.recyclerview.widget.RecyclerView;
import androidx.swiperefreshlayout.widget.SwipeRefreshLayout;
import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.schedulers.Schedulers;
import okhttp3.OkHttpClient;
import retrofit2.Retrofit;
import retrofit2.adapter.rxjava2.RxJava2CallAdapterFactory;
import retrofit2.converter.gson.GsonConverterFactory;
import sk.cll.mewsapp.data.utils.MyAdapter;
import sk.cll.mewsapp.data.utils.MyViewModel;
import sk.cll.mewsapp.data.utils.PaginationScrollListener;
import sk.cll.mewsapp.data.Photo;
import sk.cll.mewsapp.services.PhotoService;
import sk.cll.mewsapp.R;
import sk.cll.mewsapp.paging.ItemDetailActivity;
import sk.cll.mewsapp.paging.Urls;

/**
 * An activity representing a list of Items. This activity
 * has different presentations for handset and tablet-size devices. On
 * handsets, the activity presents a list of items, which when touched,
 * lead to a {@link ItemDetailActivity} representing
 * item details. On tablets, the activity presents the list of items and
 * item details side-by-side using two vertical panes.
 */
public class ItemListActivity extends AppCompatActivity implements SwipeRefreshLayout.OnRefreshListener {

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

    }

    public void downloadNewData() {
        mCompositeDisposable.add(service.getPhotos(mPhotos.size(), LIMIT)
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeOn(Schedulers.io())
                .subscribe(this::onSuccess, this::onError));
    }

    public void onSuccess(List<Photo> photos) {
        RecyclerView recyclerView = findViewById(R.id.item_list);
        assert recyclerView != null;
//                        setupRecyclerView((RecyclerView) recyclerView);


        mPhotos.addAll(photos);
//        recyclerView.setAdapter(new MyAdapter(mPhotos, this, mTwoPane));
        recyclerView.getAdapter().notifyDataSetChanged();
        Toast.makeText(ItemListActivity.this, "on next", Toast.LENGTH_SHORT).show();
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
        Toast.makeText(ItemListActivity.this, "on load more items", Toast.LENGTH_SHORT).show();
        downloadNewData();
    }

    @Override
    public void onRefresh() {
        Toast.makeText(ItemListActivity.this, "on refresh", Toast.LENGTH_SHORT).show();

    }

    public void onError(Throwable error) {
        Toast.makeText(ItemListActivity.this, error.getLocalizedMessage(), Toast.LENGTH_SHORT).show();
    }

    //todo
    @Override
    protected void onDestroy() {
        super.onDestroy();
    }
}
