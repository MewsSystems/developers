package com.mews.task.ui

import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.paging.PagedListAdapter
import androidx.recyclerview.widget.DiffUtil
import androidx.recyclerview.widget.RecyclerView
import com.bumptech.glide.Glide
import com.mews.task.R
import com.mews.task.data.Item
import kotlinx.android.extensions.LayoutContainer
import kotlinx.android.synthetic.main.viewholder_item.*

class ItemAdapter(
    private val onClick: (Item) -> Unit
) : PagedListAdapter<Item, ItemViewHolder>(DIFF_CALLBACK) {

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): ItemViewHolder {
        val inflater = LayoutInflater.from(parent.context)
        val view = inflater.inflate(R.layout.viewholder_item, parent, false)
        return ItemViewHolder(view, onClick)
    }

    override fun onBindViewHolder(holder: ItemViewHolder, position: Int) {
        getItem(position)?.let { item ->
            holder.bind(item)
        }
    }

    companion object {
        private val DIFF_CALLBACK = object : DiffUtil.ItemCallback<Item>() {

            override fun areItemsTheSame(oldItem: Item, newItem: Item): Boolean {
                return oldItem.id == newItem.id
            }

            override fun areContentsTheSame(oldItem: Item, newItem: Item): Boolean {
                return oldItem == newItem
            }
        }
    }
}

class ItemViewHolder(
    override val containerView: View,
    private val onClick: (Item) -> Unit
) : RecyclerView.ViewHolder(containerView), LayoutContainer {

    fun bind(item: Item) {
        item_title_view.text = item.title

        containerView.setOnClickListener {
            onClick(item)
        }
        Glide.with(containerView)
            .load(item.thumbUrl)
            .error(R.drawable.skull_crossbones_outline)
            .placeholder(R.drawable.loading_wheel)
            .centerCrop()
            .into(item_image_view)
    }
}