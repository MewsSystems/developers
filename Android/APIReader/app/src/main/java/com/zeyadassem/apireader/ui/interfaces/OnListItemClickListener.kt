package com.zeyadassem.apireader.ui.interfaces

import java.text.FieldPosition

interface OnListItemClickListener<T> {
    fun onItemClicked(item: T?)
}