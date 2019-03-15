package com.zeyadassem.apireader.ui.activities

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.util.Log
import com.zeyadassem.apireader.R
import com.zeyadassem.apireader.network.models.PhotoModel
import com.zeyadassem.apireader.ui.fragments.PhotoDetailsFragment
import com.zeyadassem.apireader.ui.utils.AppConstants

class DetailsActivity : AppCompatActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_details)

        if(intent.getParcelableExtra<PhotoModel>(AppConstants.PHOTO_MODEL_KEY) != null){
            val photoModel = intent.getParcelableExtra<PhotoModel>(AppConstants.PHOTO_MODEL_KEY)
            val detailsFragment = PhotoDetailsFragment.newInstance(photoModel?.url, photoModel?.title, photoModel?.id)
            supportFragmentManager.beginTransaction().replace(R.id.detailsContainer, detailsFragment).commit()
        }
    }
}
