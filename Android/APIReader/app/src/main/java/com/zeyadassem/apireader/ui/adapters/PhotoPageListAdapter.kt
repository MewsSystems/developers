package com.zeyadassem.apireader.ui.adapters

import android.view.LayoutInflater
import android.view.View
import android.view.View.VISIBLE
import android.view.ViewGroup
import android.widget.ImageView
import android.widget.ProgressBar
import android.widget.TextView
import androidx.paging.PagedListAdapter
import androidx.recyclerview.widget.DiffUtil
import androidx.recyclerview.widget.RecyclerView
import com.squareup.picasso.Picasso
import com.zeyadassem.apireader.R
import com.zeyadassem.apireader.network.models.PhotoModel
import com.zeyadassem.apireader.ui.interfaces.OnListItemClickListener
import com.zeyadassem.apireader.ui.utils.AppConstants
import kotlinx.android.synthetic.main.photo_list_footer.view.*

class PhotoPageListAdapter(private val retry: () -> Unit,
                           private val listener: OnListItemClickListener<PhotoModel>
                            ): PagedListAdapter<PhotoModel, RecyclerView.ViewHolder>(PhotosDiffCallback){

    private val DATA_VIEW_TYPE = 1
    private val FOOTER_VIEW_TYPE = 2

    private var state = AppConstants.LOADING_STATUS


    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): RecyclerView.ViewHolder {
        when (viewType){
            DATA_VIEW_TYPE -> return PhotoViewHolder(LayoutInflater.from(parent.context).inflate(R.layout.photo_list_item, parent, false))
            FOOTER_VIEW_TYPE -> return ListFooterViewHolder(LayoutInflater.from(parent.context).inflate(R.layout.photo_list_footer, parent, false), retry)
            else-> return ListFooterViewHolder(LayoutInflater.from(parent.context).inflate(R.layout.photo_list_footer, parent, false), retry)
        }

    }

    override fun onBindViewHolder(holder: RecyclerView.ViewHolder, position: Int) {
        if (getItemViewType(position) == DATA_VIEW_TYPE){
            if(getItem(position)?.title != null){
                (holder as PhotoViewHolder).titleTv.text = getItem(position)?.title + " - "+getItem(position)?.id
            }

            if(getItem(position)?.thumbnailUrl != null){
                Picasso.get()
                    .load(getItem(position)?.thumbnailUrl)
                    .into((holder as PhotoViewHolder).photoIg)
            }
        }
        else (holder as ListFooterViewHolder).bind(state)
    }




    companion object {
        val PhotosDiffCallback = object : DiffUtil.ItemCallback<PhotoModel>() {
            override fun areItemsTheSame(oldItem: PhotoModel, newItem: PhotoModel): Boolean {
                return oldItem.title == newItem.title
            }

            override fun areContentsTheSame(oldItem: PhotoModel, newItem: PhotoModel): Boolean {
                return oldItem == newItem
            }
        }
    }

    override fun getItemViewType(position: Int): Int {
        return if (position < super.getItemCount()) DATA_VIEW_TYPE else FOOTER_VIEW_TYPE
    }

    override fun getItemCount(): Int {
        return super.getItemCount() + if (hasFooter()) 1 else 0
    }

    private fun hasFooter(): Boolean {
        return super.getItemCount() != 0 && (state == AppConstants.LOADING_STATUS || state == AppConstants.ERROR_STATUS)
    }


    class ListFooterViewHolder(rootView: View, retry: () -> Unit) : RecyclerView.ViewHolder(rootView) {
        var progressBar = rootView.findViewById<ProgressBar>(R.id.footerProgressBar)
        var errorTv = rootView.findViewById<TextView>(R.id.errorTv)

        fun bind(status: String?) {
            progressBar.visibility = if (status == AppConstants.LOADING_STATUS) VISIBLE else View.INVISIBLE
            errorTv.visibility = if (status == AppConstants.ERROR_STATUS) VISIBLE else View.INVISIBLE

        }

        init {
            errorTv.setOnClickListener { retry() }
        }


    }

    inner class PhotoViewHolder(rootView: View): RecyclerView.ViewHolder(rootView){
        var photoIg = rootView.findViewById<ImageView>(R.id.photoIg)
        var titleTv = rootView.findViewById<TextView>(R.id.titleTv)
        init {
            rootView.setOnClickListener{
                listener.onItemClicked(getItem(layoutPosition))
            }
        }
    }

    fun setState(state: String) {
        this.state = state
        notifyItemChanged(getItemCount())
    }

}