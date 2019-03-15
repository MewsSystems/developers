package com.zeyadassem.apireader.di

import com.zeyadassem.apireader.ui.activities.MainActivity
import com.zeyadassem.apireader.viewmodels.PhotoViewModel
import dagger.Component
import javax.inject.Singleton


@Singleton
@Component(modules=[RetrofitModule::class])
interface PhotoRepoComponent {
    fun inject(photoViewModel: PhotoViewModel)
}