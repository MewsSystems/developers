package com.mews.task.ui

import android.util.Log
import android.view.View
import com.google.android.material.snackbar.Snackbar
import com.mews.task.R
import com.mews.task.data.NetworkState
import kotlinx.android.extensions.LayoutContainer
import kotlinx.android.synthetic.main.fragment_list.*

class NetworkStateRenderer(
    override val containerView: View
) : LayoutContainer {

    private var hasData = false

    fun render(state: NetworkState) {
        Log.i(TAG, state.toString())

        when (state) {
            NetworkState.Loading -> onLoading()
            NetworkState.Success -> onSuccess()
            is NetworkState.Error -> onError(state.throwable)
        }
    }

    private fun onLoading() {
        if (!list_refresh_layout.isRefreshing) {
            list_progress_bar.visibility = View.VISIBLE
        }
    }

    private fun onSuccess() {
        hasData = true
        list_refresh_layout.isRefreshing = false
        list_progress_bar.visibility = View.GONE
        list_empty_message.visibility = View.GONE
    }

    private fun onError(throwable: Throwable) {
        list_refresh_layout.isRefreshing = false
        list_progress_bar.visibility = View.GONE

        if (!hasData) {
            list_empty_message.setText(R.string.list_empty_message)
            list_empty_message.visibility = View.VISIBLE
        }
        val message = throwable.message ?: throwable::class.java.simpleName
        Snackbar.make(containerView, message, Snackbar.LENGTH_LONG).show()
    }

    companion object {
        private const val TAG = "NetworkStateRenderer"
    }
}