package com.zeyadassem.apireader.ui.activities

import android.content.Intent
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.view.View
import androidx.lifecycle.Observer
import androidx.lifecycle.ViewModelProviders
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.zeyadassem.apireader.R
import com.zeyadassem.apireader.network.models.PhotoModel
import com.zeyadassem.apireader.ui.adapters.PhotoPageListAdapter
import com.zeyadassem.apireader.ui.fragments.PhotoDetailsFragment
import com.zeyadassem.apireader.ui.interfaces.OnListItemClickListener
import com.zeyadassem.apireader.ui.utils.AppConstants
import com.zeyadassem.apireader.viewmodels.PhotoViewModel
import kotlinx.android.synthetic.main.activity_main.*
import kotlinx.android.synthetic.main.error_layout.*

class MainActivity : AppCompatActivity(), OnListItemClickListener<PhotoModel> {


    lateinit var photoViewModel: PhotoViewModel
    private lateinit var photoListAdapter: PhotoPageListAdapter
    var twoPane = false


    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        setTwoPane()

        photoViewModel = ViewModelProviders.of(this).get(PhotoViewModel::class.java)
        initAdapter()
        initState()

    }

    private fun setTwoPane() {
        twoPane = item_detail_container != null
    }



    private fun initAdapter() {
        if(AppConstants.checkInternetConnection(this)){
            photoListAdapter = PhotoPageListAdapter ({photoViewModel.retry()}, this)
            photoListRv.layoutManager = LinearLayoutManager(this, RecyclerView.VERTICAL, false)
            photoListRv.adapter = photoListAdapter
            photoViewModel.photoListLiveData.observe(this, Observer {
                progressBar.visibility = View.GONE
                errorLayout.visibility = View.GONE
                photoListRv.visibility = View.VISIBLE
                photoListAdapter.submitList(it)
            })
        }else{
            progressBar.visibility = View.GONE
            errorLayout.visibility = View.VISIBLE
            photoListRv.visibility = View.GONE
            refreshIv.setOnClickListener{initAdapter()}
        }

    }

    private fun initState() {
        photoViewModel.progressLiveData.observe(this, Observer { state ->
            if (!photoViewModel.listIsEmpty()) {
                photoListAdapter.setState(state ?: AppConstants.LOADED_STATUS)
            }
        })
    }

    override fun onItemClicked(photoModel: PhotoModel?) {
        if(twoPane){
            val detailsFragment = PhotoDetailsFragment.newInstance(photoModel?.url, photoModel?.title, photoModel?.id)
            supportFragmentManager.beginTransaction().replace(R.id.item_detail_container, detailsFragment).commit()

        }else{
            val detailsIntent = Intent(this, DetailsActivity::class.java)
            detailsIntent.putExtra(AppConstants.PHOTO_MODEL_KEY, photoModel)
            startActivity(detailsIntent)
        }
    }


}
