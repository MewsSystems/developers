package com.zeyadassem.apireader.ui.fragments


import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import com.squareup.picasso.Picasso

import com.zeyadassem.apireader.R
import kotlinx.android.synthetic.main.fragment_photo_details.*


private const val ARG_IMG_URL = "img_url"
private const val ARG_TITLE = "title"
private const val ARG_ID = "id"


class PhotoDetailsFragment : Fragment() {
    private var imgUrl: String? = null
    private var title: String? = null
    private var id: Int? = null

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        arguments?.let {
            imgUrl = it.getString(ARG_IMG_URL)
            title = it.getString(ARG_TITLE)
            id = it.getInt(ARG_ID)
        }
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        return inflater.inflate(R.layout.fragment_photo_details, container, false)
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)
        if(imgUrl != null){
            Picasso.get()
                .load(imgUrl)
                .into(photoDetailsIv)
        }

        if(title != null){
            titleTv.text = title+" - $id"
        }
    }


    companion object {
        @JvmStatic
        fun newInstance(imgUrl: String?, title: String?, id: Int?) =
            PhotoDetailsFragment().apply {
                arguments = Bundle().apply {
                    putString(ARG_IMG_URL, imgUrl)
                    putString(ARG_TITLE, title)
                    putInt(ARG_ID, id?:0)
                }
            }
    }
}
