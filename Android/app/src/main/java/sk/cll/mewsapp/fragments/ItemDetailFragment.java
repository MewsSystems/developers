package sk.cll.mewsapp.fragments;

import android.app.Activity;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;

import com.squareup.picasso.Picasso;

import androidx.fragment.app.Fragment;
import androidx.lifecycle.ViewModelProviders;
import sk.cll.mewsapp.R;
import sk.cll.mewsapp.data.Photo;
import sk.cll.mewsapp.data.utils.MyViewModel;


public class ItemDetailFragment extends Fragment {
    private Photo mPhoto;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setHasOptionsMenu(true);
        MyViewModel model = ViewModelProviders.of(getActivity()).get(MyViewModel.class);
        mPhoto = model.getSelected();
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        View rootView = inflater.inflate(R.layout.item_detail, container, false);

        if (mPhoto != null) {
            ((TextView) rootView.findViewById(R.id.tv_id))
                    .setText(String.format("ID: %d", mPhoto.getId()));
            ((TextView) rootView.findViewById(R.id.tv_albumId))
                    .setText(String.format("Album ID: %d", mPhoto.getAlbumId()));
            ((TextView) rootView.findViewById(R.id.tv_title))
                    .setText(mPhoto.getTitle());
            Activity activity = this.getActivity();
            if (activity != null) {
                Picasso.get().load(mPhoto.getUrl())
                        .placeholder(R.drawable.placeholder_large)
                        .into(((ImageView) rootView.findViewById(R.id.img_photo)));
            }
        }
        return rootView;
    }
}
