package sk.cll.mewsapp;

import android.content.res.Configuration;
import android.os.Bundle;
import android.view.View;
import android.widget.Toast;

import java.util.ArrayList;
import java.util.List;

import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.Toolbar;
import androidx.recyclerview.widget.RecyclerView;
import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.schedulers.Schedulers;
import okhttp3.OkHttpClient;
import retrofit2.Retrofit;
import retrofit2.adapter.rxjava2.RxJava2CallAdapterFactory;
import retrofit2.converter.gson.GsonConverterFactory;

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
    /**
     * Whether or not the activity is in two-pane mode, i.e. running on a tablet
     * device.
     */
    private boolean mTwoPane;
    private List<Photo> mPhotos;
    private CompositeDisposable mCompositeDisposable;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_item_list);

        Toolbar toolbar = findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);
        toolbar.setTitle(getTitle());

        mTwoPane = getResources().getConfiguration().orientation == Configuration.ORIENTATION_LANDSCAPE;

        PhotoService service = new Retrofit.Builder()
                .baseUrl(BASE_URL)
                .addConverterFactory(GsonConverterFactory.create())
                .client(new OkHttpClient.Builder().build())
                .addConverterFactory(GsonConverterFactory.create())
                .addCallAdapterFactory(RxJava2CallAdapterFactory.create())
                .build().create(PhotoService.class);

        mCompositeDisposable = new CompositeDisposable();
        mCompositeDisposable.add(service.getPhotos(0, LIMIT)
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeOn(Schedulers.io())
                .subscribe(this::onSuccess, this::onError));


//        View recyclerView = findViewById(R.id.item_list);
//        assert recyclerView != null;
//        setupRecyclerView((RecyclerView) recyclerView);

    }

    public void onSuccess(List<Photo> photos) {
        View recyclerView = findViewById(R.id.item_list);
        assert recyclerView != null;
//                        setupRecyclerView((RecyclerView) recyclerView);
        mPhotos = new ArrayList<>(photos);
        ((RecyclerView) recyclerView).setAdapter(new MyAdapter(mPhotos, getApplicationContext(), mTwoPane));
        Toast.makeText(ItemListActivity.this, "on next", Toast.LENGTH_SHORT).show();
    }

    public void onError(Throwable error) {
        Toast.makeText(ItemListActivity.this, error.getLocalizedMessage(), Toast.LENGTH_SHORT).show();
    }

    //todo
    @Override
    protected void onDestroy() {
        super.onDestroy();
        mCompositeDisposable.clear(); // do not send event after activity has been destroyed
    }
}
