package com.mews.task.ui

import androidx.fragment.app.Fragment
import androidx.lifecycle.*
import toothpick.Scope
import toothpick.Toothpick

inline fun <reified VM : ViewModel> Fragment.getViewModel(crossinline factory: (Scope) -> VM): VM {
    val scope = Toothpick.openScope(activity?.applicationContext)

    val factory = object : ViewModelProvider.Factory {
        override fun <VM : ViewModel?> create(modelClass: Class<VM>): VM {
            return factory(scope) as VM
        }
    }
    return ViewModelProviders.of(this, factory).get(VM::class.java)
}