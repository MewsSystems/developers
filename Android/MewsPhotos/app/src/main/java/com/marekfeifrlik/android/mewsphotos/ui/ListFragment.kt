package com.marekfeifrlik.android.mewsphotos.ui

import android.content.Context
import android.net.ConnectivityManager
import android.net.NetworkInfo
import android.os.Bundle
import android.util.Log
import android.view.*
import androidx.appcompat.app.AlertDialog
import androidx.fragment.app.Fragment
import androidx.lifecycle.LifecycleOwner
import androidx.lifecycle.Observer
import androidx.lifecycle.ViewModelProviders
import androidx.recyclerview.widget.LinearLayoutManager
import com.marekfeifrlik.android.mewsphotos.data.MyAdapter
import com.marekfeifrlik.android.mewsphotos.R
import com.marekfeifrlik.android.mewsphotos.data.SharedViewModel
import com.marekfeifrlik.android.mewsphotos.api.PhotosApi
import com.marekfeifrlik.android.mewsphotos.data.PhotoEntity
import io.reactivex.android.schedulers.AndroidSchedulers
import io.reactivex.internal.schedulers.IoScheduler
import kotlinx.android.synthetic.main.fragment_list.*

class ListFragment : Fragment() {

    private lateinit var model: SharedViewModel

    private val photosApi by lazy {
        PhotosApi.create()
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        val observer = Observer<PhotoEntity> { }

        super.onCreate(savedInstanceState)
        activity?.let {
            model = ViewModelProviders.of(it).get(SharedViewModel::class.java)
        }
        model.selected.observe(activity as LifecycleOwner, observer)
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        setHasOptionsMenu(true)
        return inflater.inflate(R.layout.fragment_list, container, false)
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)
        recyclerView.layoutManager = LinearLayoutManager(activity, LinearLayoutManager.HORIZONTAL, false)

        if (isNetworkAvailable()) {
            loadData()
        } else {
            showAlertDialog()
        }
    }


    private fun isNetworkAvailable(): Boolean {
        val connectivityManager = activity!!.getSystemService(Context.CONNECTIVITY_SERVICE)
        return if (connectivityManager is ConnectivityManager) {
            val networkInfo: NetworkInfo? = connectivityManager.activeNetworkInfo
            networkInfo?.isConnected ?: false
        } else false
    }

    private fun showAlertDialog() {
        val builder = AlertDialog.Builder(activity!!)
        builder.setTitle("No internet Connection")
        builder.setMessage("Please turn on internet connection to continue")
        builder.setNegativeButton(
            "close"

        ) { dialog, button -> activity!!.finish() }
        val alertDialog = builder.create()
        alertDialog.show()
    }

    private fun loadData() {
        val json = photosApi.getPhotos()
        json.observeOn(AndroidSchedulers.mainThread()).subscribeOn(IoScheduler()).subscribe {
            recyclerView.adapter = MyAdapter(it) { photoEntity ->
                Log.d("logged", "${photoEntity.title} Clicked")
                model.select(photoEntity)
            }
        }
    }

}