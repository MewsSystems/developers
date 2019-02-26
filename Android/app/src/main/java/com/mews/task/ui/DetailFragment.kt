package com.mews.task.ui

import android.content.res.Configuration
import android.os.Bundle
import android.view.Gravity
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.core.content.ContextCompat
import androidx.fragment.app.Fragment
import androidx.transition.Slide
import com.bumptech.glide.Glide
import com.mews.task.R
import com.mews.task.data.Item
import kotlinx.android.synthetic.main.fragment_detail.*

class DetailFragment : Fragment() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enterTransition = Slide(Gravity.LEFT)
        exitTransition = Slide(Gravity.LEFT)
    }

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        super.onCreateView(inflater, container, savedInstanceState)
        return inflater.inflate(R.layout.fragment_detail, container, false)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        val bundle = arguments ?: Bundle.EMPTY

        if (bundle.containsKey(ITEM_KEY)) {
            val item = bundle[ITEM_KEY] as Item
            populate(item)
        }
        setupToolbar()
    }

    private fun setupToolbar() {
        if (resources.configuration.orientation == Configuration.ORIENTATION_LANDSCAPE) {
            detail_toolbar.visibility = View.GONE

        } else {
            detail_toolbar.setNavigationOnClickListener {
                fragmentManager?.popBackStackImmediate()
            }
            detail_toolbar.navigationIcon = ContextCompat.getDrawable(requireContext(), R.drawable.ic_close)
        }
    }

    private fun populate(item: Item) {
        detail_title_view.text = item.title

        Glide.with(this)
            .load(item.url)
            .error(R.drawable.skull_crossbones_outline)
            .placeholder(R.drawable.loading_wheel)
            .centerCrop()
            .into(detail_image_view)
    }

    companion object {
        private const val ITEM_KEY = "detail.item"
        const val TAG = "detail-fragment"

        fun getInstance(item: Item): DetailFragment {
            return DetailFragment().apply {
                arguments = Bundle().apply {
                    putParcelable(ITEM_KEY, item)
                }
            }
        }
    }
}