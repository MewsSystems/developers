package com.marekfeifrlik.android.mewsphotos.ui

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import androidx.lifecycle.Observer
import androidx.lifecycle.ViewModelProviders
import com.marekfeifrlik.android.mewsphotos.R
import com.marekfeifrlik.android.mewsphotos.data.SharedViewModel
import com.squareup.picasso.Picasso
import kotlinx.android.synthetic.main.fragment_detail.image_view
import org.koin.androidx.viewmodel.ext.android.viewModel

class MainActivity : AppCompatActivity() {

    val model: SharedViewModel by viewModel()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)
        supportFragmentManager.beginTransaction().add(
            R.id.layout_detail,
            DetailFragment()
        ).commit()
        supportFragmentManager.beginTransaction().add(
            R.id.layout_list,
            ListFragment()
        ).commit()

        val sharedViewModel = ViewModelProviders.of(this).get(SharedViewModel::class.java)

        sharedViewModel.selected.observe(this, Observer { it ->
            it?.let {
                Picasso.with(this).load(it.url).fit().centerCrop()
                    .into(image_view)
            }
        })
    }

}
