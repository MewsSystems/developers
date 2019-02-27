package com.mews.task.ui

import android.content.res.Configuration
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.core.content.ContextCompat
import androidx.fragment.app.Fragment
import androidx.lifecycle.Observer
import androidx.recyclerview.widget.LinearLayoutManager
import com.mews.task.R
import com.mews.task.vm.ListViewModel
import kotlinx.android.synthetic.main.fragment_detail.*
import kotlinx.android.synthetic.main.fragment_list.*

class ListFragment : Fragment() {

    private lateinit var networkStateRenderer: NetworkStateRenderer
    private lateinit var listViewModel: ListViewModel

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        super.onCreateView(inflater, container, savedInstanceState)
        return inflater.inflate(R.layout.fragment_list, container, false)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        setupToolbar()
        networkStateRenderer = NetworkStateRenderer(view)
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)

        val itemAdapter = setupItemAdapter()
        setupViews(itemAdapter)
        setupViewModel(itemAdapter)
    }

    private fun setupToolbar() {
        if (resources.configuration.orientation == Configuration.ORIENTATION_LANDSCAPE) {
            list_toolbar.visibility = View.GONE
        }
    }

    private fun setupItemAdapter(): ItemAdapter {
        return ItemAdapter { item ->
            val fragment = DetailFragment.getInstance(item)

            requireFragmentManager().apply {
                popBackStack()
                beginTransaction()
                    .replace(R.id.frame_detail, fragment, DetailFragment.TAG)
                    .addToBackStack(null)
                    .commit()
            }
        }
    }

    private fun setupViews(itemAdapter: ItemAdapter) {
        list_refresh_layout.setOnRefreshListener {
            listViewModel.reload()
        }
        list_recycler_view.apply {
            adapter = itemAdapter
            layoutManager = LinearLayoutManager(context)
        }
    }

    private fun setupViewModel(itemAdapter: ItemAdapter) {
        listViewModel = getViewModel { scope -> ListViewModel.create(scope) }

        listViewModel.items.observe(viewLifecycleOwner, Observer { list ->
            itemAdapter.submitList(list)
        })
        listViewModel.networkState.observe(viewLifecycleOwner, Observer { state ->
            networkStateRenderer.render(state)
        })
    }
}