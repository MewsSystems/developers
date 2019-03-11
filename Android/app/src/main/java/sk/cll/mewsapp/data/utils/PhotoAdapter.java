package sk.cll.mewsapp.data.utils;

import android.view.View;
import android.widget.ImageView;
import android.widget.TextView;

import com.google.android.material.button.MaterialButton;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;
import sk.cll.mewsapp.R;

public class PhotoAdapter {
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


}
