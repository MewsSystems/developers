package com.mews.task

import android.Manifest
import android.os.Bundle
import androidx.appcompat.app.AppCompatActivity
import com.mews.task.ui.ListFragment

class MainActivity : AppCompatActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        super.setContentView(R.layout.activity_main)
    }

    override fun onStart() {
        super.onStart()
        requestPermission(Manifest.permission.INTERNET)
    }
}
