package com.mews.task.net

import androidx.paging.DataSource
import com.mews.task.data.Item

abstract class ItemDataSourceFactory : DataSource.Factory<Int, Item>()