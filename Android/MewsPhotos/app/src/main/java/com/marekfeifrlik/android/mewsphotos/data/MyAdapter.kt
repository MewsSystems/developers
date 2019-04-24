package com.marekfeifrlik.android.mewsphotos.data

import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ImageView
import android.widget.LinearLayout
import androidx.recyclerview.widget.RecyclerView
import com.marekfeifrlik.android.mewsphotos.R
import com.squareup.picasso.Picasso
import kotlinx.android.synthetic.main.item_photo.view.*

class MyAdapter(val items: List<PhotoEntity>, val listener: (PhotoEntity) -> Unit) : RecyclerView.Adapter<ViewHolder>() {

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int) =
        ViewHolder(
            LayoutInflater.from(parent.context).inflate(
                R.layout.item_photo,
                parent,
                false
            )
        )

    override fun onBindViewHolder(holder: ViewHolder, position: Int) {
        holder.bind(items[position], listener)
        holder.itemDetail.visibility = View.INVISIBLE
        holder.imageView.setOnLongClickListener { view ->
            holder.itemDetail.visibility = View.VISIBLE
            true
        }
    }

    override fun getItemCount() = items.size
}

class ViewHolder(itemView: View) : RecyclerView.ViewHolder(itemView) {
    var imageView: ImageView = itemView.findViewById(R.id.image_view) as ImageView
    var itemDetail: LinearLayout = itemView.findViewById(R.id.layout_item_show_ids) as LinearLayout

    fun bind(item: PhotoEntity, listener: (PhotoEntity) -> Unit) = with(itemView) {

        Picasso.with(context).load(item.thumbnailUrl).fit().centerCrop()
            .placeholder(R.drawable.design_ic_visibility)
            .error(R.drawable.ic_mtrl_chip_close_circle)
            .into(image_view)

        txt_title.text = "Title: " + item.title
        txt_photo_id.text = "Photo ID: " + Integer.toString(item.id)
        txt_album_id.text = "Album ID: " + Integer.toString(item.albumId)

        setOnClickListener { listener(item) }
    }

}