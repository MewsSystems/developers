package com.example.mews.myapplication

import android.content.Context
import android.support.v7.widget.RecyclerView
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.view.animation.AnimationUtils
import android.widget.ImageView
import android.widget.LinearLayout
import android.widget.TextView
import com.bumptech.glide.Glide
import android.os.Handler
import android.text.method.ScrollingMovementMethod

class ImageAdapter(private val imageList: MutableList<Image>?, private val context: Context,
                   private var mlistener: CustomItemClickListener?) :
    RecyclerView.Adapter<ImageAdapter.ImageViewHolder>() {

    interface CustomItemClickListener {
        fun onItemClick(context: Context, imagePath: String)
        fun onItemScrollDown(currentPosition: Int, itemsCount: Int, offsetPosition: Int = 0)
    }

    fun setCustomItemClickListener(listener: CustomItemClickListener) {
        mlistener = listener
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): ImageAdapter.ImageViewHolder {
        val v = LayoutInflater.from(parent.context).inflate(R.layout.list_layout_images, parent, false)
        return ImageViewHolder(v)
    }

    override fun onBindViewHolder(holder: ImageViewHolder, position: Int) {
        val pos = holder.adapterPosition
        val image = imageList?.get(pos)
        holder.textViewName.text = image?.title
        holder.textViewName.movementMethod = ScrollingMovementMethod()
        // holder.textViewRealName.setText(image?.url)
        holder.textViewAlbumId.text = image!!.albumId.toString()
        holder.textViewImageId.text = image.id.toString()

        Glide.with(context).load(image.thumbnailUrl).into(holder.imageView)
        holder.linearLayout.visibility = View.GONE

        //if the position is equals to the item position which is to be expanded
        if (!ImageAdapter.collapse && ImageAdapter.currentPosition == pos) {
            //creating an animation
            val slideDown = AnimationUtils.loadAnimation(context, R.anim.slide_down)

            //toggling visibility
            holder.linearLayout.visibility = View.VISIBLE

            //adding sliding effect
            holder.linearLayout.startAnimation(slideDown)
        }

        holder.imageView.setOnClickListener {
            image.url.let { it1 -> mlistener?.onItemClick(context, it1) }
        }

        holder.textViewName.setOnClickListener {
            //if the position is equals to the item position which was expanded and now going to be collapsed
            if (ImageAdapter.currentPosition == pos) {
                ImageAdapter.collapse = true
                //creating an animation
                val slideUp = AnimationUtils.loadAnimation(context, R.anim.slide_up)

                //adding sliding effect
                holder.linearLayout.startAnimation(slideUp)

                //toggling visibility
                Run.after(500) {
                    //toggling visibility
                    holder.linearLayout.visibility = View.GONE
                }
            }
        }

        holder.textViewDetail.setOnClickListener {
            //getting the position of the item to expand it
            ImageAdapter.currentPosition = pos
            ImageAdapter.collapse = false

            //reloding the list
            notifyDataSetChanged()

            //if the position is equals to the last visible item
            this.mlistener?.onItemScrollDown(ImageAdapter.currentPosition, itemCount, 2)
        }
    }

    override fun getItemCount(): Int {
        return imageList!!.size
    }

    inner class ImageViewHolder(itemView: View) : RecyclerView.ViewHolder(itemView) {
        var imageView: ImageView = itemView.findViewById(R.id.imageView) as ImageView
        var textViewName: TextView = itemView.findViewById<View>(R.id.textViewName) as TextView
        var textViewDetail: TextView = itemView.findViewById<View>(R.id.textViewDetail) as TextView
        // var textViewRealName: TextView
        var textViewAlbumId: TextView = itemView.findViewById<View>(R.id.textViewAlbumId) as TextView
        var textViewImageId: TextView = itemView.findViewById<View>(R.id.textViewImageId) as TextView
        var linearLayout: LinearLayout = itemView.findViewById<View>(R.id.linearLayout) as LinearLayout

        init {
            // textViewRealName = itemView.findViewById<View>(R.id.textViewRealName) as TextView
            itemView.setOnClickListener {mlistener?.onItemClick(context, "imageUrl")}
            itemView.setOnClickListener {mlistener?.onItemScrollDown(0, 0, 0)}
        }
    }

    companion object {

        private var currentPosition = 0
        private var collapse = false
    }
}

class Run {
    companion object {
        fun after(delay: Long, process: () -> Unit) {
            Handler().postDelayed({
                process()
            }, delay)
        }
    }
}
