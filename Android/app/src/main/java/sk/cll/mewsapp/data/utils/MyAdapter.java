package sk.cll.mewsapp.data.utils;

import android.content.Context;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;

import com.google.android.material.button.MaterialButton;
import com.google.gson.Gson;
import com.squareup.picasso.Picasso;

import java.util.List;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;
import androidx.fragment.app.Fragment;
import androidx.fragment.app.FragmentManager;
import androidx.lifecycle.ViewModelProviders;
import androidx.recyclerview.widget.RecyclerView;
import sk.cll.mewsapp.R;
import sk.cll.mewsapp.data.Photo;
import sk.cll.mewsapp.fragments.ItemDetailFragment;

public class MyAdapter extends RecyclerView.Adapter<MyAdapter.MyViewHolder> {
    private List<Photo> mPhotos;
    private Context mContext;

    public MyAdapter(List<Photo> photos, Context context) {
        mPhotos = photos;
        mContext = context;
    }

    class MyViewHolder extends RecyclerView.ViewHolder {

        private final TextView title;
        private final TextView id;
        private final TextView albumId;
        private final ImageView thumbnail;
        private final MaterialButton details;

        public MyViewHolder(@NonNull View itemView) {
            super(itemView);
            title = itemView.findViewById(R.id.tv_title);
            id = itemView.findViewById(R.id.tv_id);
            albumId = itemView.findViewById(R.id.tv_albumId);
            thumbnail = itemView.findViewById(R.id.img_thumbnail);
            details = itemView.findViewById(R.id.btn_details);
        }
    }

    @NonNull
    @Override
    public MyViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext())
                .inflate(R.layout.item_list_content, parent, false);
        return new MyViewHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull MyViewHolder holder, final int position) {
        holder.title.setText(mPhotos.get(position).getTitle());
        holder.id.setText(String.format("Photo id: %d", mPhotos.get(position).getId()));
        holder.albumId.setText(String.format("Album id: %d", mPhotos.get(position).getAlbumId()));
        Picasso.get().load(mPhotos.get(position).getThumbnailUrl())
                .placeholder(R.drawable.placeholder_small).into(holder.thumbnail);

        holder.itemView.setTag(mPhotos.get(position));
        if (mPhotos.get(position).isExpanded()) {
            holder.id.setVisibility(View.VISIBLE);
            holder.albumId.setVisibility(View.VISIBLE);
            holder.details.setVisibility(View.VISIBLE);

        } else {
            holder.id.setVisibility(View.GONE);
            holder.albumId.setVisibility(View.GONE);
            holder.details.setVisibility(View.GONE);
        }
//        holder.itemView.setOnClickListener(mOnClickListener);

        holder.itemView.setOnClickListener(v -> {
            if (mPhotos.get(position).isExpanded()) {
                mPhotos.get(position).setExpanded(false);
                v.findViewById(R.id.tv_id).setVisibility(View.GONE);
                v.findViewById(R.id.tv_albumId).setVisibility(View.GONE);
                v.findViewById(R.id.btn_details).setVisibility(View.GONE);
            } else {
                mPhotos.get(position).setExpanded(true);
                v.findViewById(R.id.tv_id).setVisibility(View.VISIBLE);
                v.findViewById(R.id.tv_albumId).setVisibility(View.VISIBLE);
                v.findViewById(R.id.btn_details).setVisibility(View.VISIBLE);
            }
        });

        holder.details.setOnClickListener(view -> {

            MyViewModel model = ViewModelProviders.of((AppCompatActivity) mContext).get(MyViewModel.class);
            model.setSelected(mPhotos.get(position));

            Gson g = new Gson();
                Bundle arguments = new Bundle();
                arguments.putString("photo", g.toJson(mPhotos.get(position)));

                ItemDetailFragment fragment = new ItemDetailFragment();
                fragment.setArguments(arguments);
                FragmentManager fm = ((AppCompatActivity) mContext).getSupportFragmentManager();
                Fragment old = fm.findFragmentByTag("detail");
                if (old != null) {
                    fm.beginTransaction().remove(old).commit();
                }
                fm.beginTransaction()
                        .addToBackStack("detail")
                        .replace(R.id.item_detail_container, fragment, "detail")
                        .commit();

        });
    }

    @Override
    public int getItemCount() {
        return mPhotos == null ? 0 : mPhotos.size();
    }
}
