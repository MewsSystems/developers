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


class MarvelAdapter(private val marvelList: MutableList<Marvel>?, private val context: Context,
                    private var mlistener: CustomItemClickListener?) :
    RecyclerView.Adapter<MarvelAdapter.MarvelViewHolder>() {

    interface CustomItemClickListener {
        fun onItemClickMarvel(context: Context, imagePath: String)
        fun onItemScrollDownMarvel(currentPosition: Int, itemsCount: Int, offsetPosition: Int = 0)
    }

    fun setCustomItemClickListener(listener: CustomItemClickListener) {
        mlistener = listener
    }

    object MySingleton {
        @Synchronized fun getInstance(
            marvelList: MutableList<Marvel>?,
            applicationContext: Context,
            listener2: CustomItemClickListener?
        ): MarvelAdapter {
            return MarvelAdapter(marvelList, applicationContext, listener2)
        }
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): MarvelAdapter.MarvelViewHolder {
        val v = LayoutInflater.from(parent.context).inflate(R.layout.list_layout_images, parent, false)
        return MarvelViewHolder(v)
    }

    override fun onBindViewHolder(holder: MarvelViewHolder, position: Int) {
        val pos = holder.adapterPosition
        val marvel = marvelList?.get(pos)
        holder.textViewName.text = marvel?.imageurl
        holder.textViewName.movementMethod = ScrollingMovementMethod()
        holder.textViewBio.text = marvel?.bio
        holder.textViewAlbumId.text = marvel!!.team
        holder.textViewImageId.text = marvel.firstappearance

        Glide.with(context).load(marvel.imageurl).into(holder.imageView)
        holder.linearLayout.visibility = View.GONE

        //if the position is equals to the item position which is to be expanded
        if (!MarvelAdapter.collapse && MarvelAdapter.currentPosition == pos) {
            //creating an animation
            val slideDown = AnimationUtils.loadAnimation(context, R.anim.slide_down)

            //toggling visibility
            holder.linearLayout.visibility = View.VISIBLE

            //adding sliding effect
            holder.linearLayout.startAnimation(slideDown)
        }

        holder.imageView.setOnClickListener {
            marvel.imageurl.let { it1 -> mlistener?.onItemClickMarvel(context, it1) }
        }

        holder.textViewName.setOnClickListener {
            //if the position is equals to the item position which was expanded and now going to be collapsed
            if (MarvelAdapter.currentPosition == pos) {
                MarvelAdapter.collapse = true
                //creating an animation
                val slideUp = AnimationUtils.loadAnimation(context, R.anim.slide_up)

                //adding sliding effect
                holder.linearLayout.startAnimation(slideUp)

                //toggling visibility
                Run2.after(500) {
                    //toggling visibility
                    holder.linearLayout.visibility = View.GONE
                }
            }
        }

        holder.textViewDetail.setOnClickListener {
            //getting the position of the item to expand it
            MarvelAdapter.currentPosition = pos
            MarvelAdapter.collapse = false

            //reloding the list
            notifyDataSetChanged()

            //if the position is equals to the last visible item
            this.mlistener?.onItemScrollDownMarvel(MarvelAdapter.currentPosition, itemCount, 0)
        }
    }

    override fun getItemCount(): Int {
        return marvelList!!.size
    }

    inner class MarvelViewHolder(itemView: View) : RecyclerView.ViewHolder(itemView) {
        var imageView: ImageView = itemView.findViewById(R.id.imageView) as ImageView
        var textViewName: TextView = itemView.findViewById<View>(R.id.textViewName) as TextView
        var textViewDetail: TextView = itemView.findViewById<View>(R.id.textViewDetail) as TextView
        var textViewBio: TextView = itemView.findViewById<View>(R.id.textViewBio) as TextView
        var textViewAlbumId: TextView = itemView.findViewById<View>(R.id.textViewAlbumId) as TextView
        var textViewImageId: TextView = itemView.findViewById<View>(R.id.textViewImageId) as TextView
        var linearLayout: LinearLayout = itemView.findViewById<View>(R.id.linearLayout) as LinearLayout

        init {
            itemView.setOnClickListener {mlistener?.onItemClickMarvel(context, "imageUrl")}
            itemView.setOnClickListener {mlistener?.onItemScrollDownMarvel(0, 0, 0)}
        }
    }

    companion object {

        private var currentPosition = 0
        private var collapse = false
    }

}

class Run2 {
    companion object {
        fun after(delay: Long, process: () -> Unit) {
            Handler().postDelayed({
                process()
            }, delay)
        }
    }
}
