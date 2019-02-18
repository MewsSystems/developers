package com.example.mews.myapplication

import android.content.Context
import android.support.v7.app.AppCompatActivity
import android.os.Bundle
import android.os.Parcelable
import android.support.v7.widget.LinearLayoutManager
import android.support.v7.widget.RecyclerView
import android.view.Surface
import android.view.WindowManager
import android.widget.ImageView
import com.android.volley.Request
import com.android.volley.Response
import com.android.volley.toolbox.StringRequest
import com.android.volley.toolbox.Volley
import com.bumptech.glide.Glide
import org.json.JSONArray
import org.json.JSONException
import java.util.ArrayList
import android.util.Log

class MainActivity : AppCompatActivity(), ImageAdapter.CustomItemClickListener {

    var start = 0
    var limit = 30
    lateinit var recyclerView: RecyclerView
    private var adapter: ImageAdapter? = null
    private var imageList: MutableList<Image>? = null
    var imageViewDetail: ImageView? = null
    private var listener: ImageAdapter.CustomItemClickListener? = null
    lateinit var layoutManager: LinearLayoutManager
    private var loading: Boolean = false
    private lateinit var recyclerViewState: Parcelable

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        /*
        setContentView(R.layout.activity_main)
        */
        val orientation = this@MainActivity.getRotation()
        when (orientation){
            R.integer.PORTRAIT -> setContentView(R.layout.portrait)
            R.integer.LANDSCAPE -> setContentView(R.layout.landscape)
        }

        imageViewDetail = findViewById(R.id.imageViewDetail)
        recyclerView = findViewById(R.id.recyclerView)

        recyclerView.layoutManager = LinearLayoutManager(this)
        layoutManager = recyclerView.layoutManager as LinearLayoutManager
        recyclerView.addOnScrollListener(object : RecyclerView.OnScrollListener() {
            override fun onScrolled(recyclerView: RecyclerView, dx: Int, dy: Int) {
                //check for scroll down
                if (dy > 0) {
                    val visibleItemCount = layoutManager.childCount
                    val totalItemCount = layoutManager.itemCount
                    val pastVisiblesItems = layoutManager.findFirstVisibleItemPosition()
                    if (loading) {
                        if (visibleItemCount + pastVisiblesItems >= totalItemCount) {
                            loading = false
                            // Save state
                            recyclerViewState = recyclerView.layoutManager.onSaveInstanceState()
                            // Last Item Wow. Do pagination.. i.e. fetch new data
                            start += limit
                            loadImagess(start, limit)
                        }
                    }
                }
            }
        })

        imageList = ArrayList()

        loadImagess(start, limit)
    }

    override fun onItemClick(context: Context, imagePath: String) {
        Glide.with(context).load(imagePath).into(imageViewDetail)
    }

    override fun onItemScrollDown(currentPosition: Int, itemsCount: Int, offsetPosition: Int) {
        layoutManager = recyclerView.layoutManager as LinearLayoutManager
        val lastVisiblePosition = layoutManager.findLastVisibleItemPosition()
        if (currentPosition == lastVisiblePosition) {
            layoutManager.scrollToPosition(lastVisiblePosition + offsetPosition)
        }
    }

    private fun loadImagess(start: Int, limit: Int) {
        val stringRequest = StringRequest(Request.Method.GET, "http://jsonplaceholder.typicode.com/photos?_start=$start&_limit=$limit",
            Response.Listener<String> { response ->
                try {
                    val jsonArray = JSONArray(response)
                    loading = true

                    for (i in 0 until jsonArray.length()) {
                        val obj = jsonArray.getJSONObject(i)

                        val image = Image(
                            obj.getInt("albumId"),
                            obj.getInt("id"),
                            obj.getString("title"),
                            obj.getString("url"),
                            obj.getString("thumbnailUrl")
                        )

                        imageList?.add(image)
                    }

                    adapter = ImageAdapter(imageList, applicationContext, listener)
                    if (start > 0) {
                        adapter!!.notifyDataSetChanged()
                        // Restore state
                        recyclerView.layoutManager.onRestoreInstanceState(recyclerViewState)

                    } else {
                        recyclerView.adapter = adapter
                    }
                    adapter!!.setCustomItemClickListener(this@MainActivity)

                } catch (e: JSONException) {
                    e.printStackTrace()
                }
            },
            Response.ErrorListener { error -> Log.e("Volley Error: ", error.message) })
        val requestQueue = Volley.newRequestQueue(this)
        requestQueue.add(stringRequest)
    }
}

fun AppCompatActivity.getRotation(): Int{
    val display = (baseContext.getSystemService(Context.WINDOW_SERVICE) as WindowManager).defaultDisplay
    val rotation = display.rotation
    return when(rotation){
        Surface.ROTATION_90 -> R.integer.LANDSCAPE
        Surface.ROTATION_270 -> R.integer.LANDSCAPE
        Surface.ROTATION_180 -> R.integer.PORTRAIT
        Surface.ROTATION_0 -> R.integer.PORTRAIT
        else ->
            R.integer.PORTRAIT
    }
}
