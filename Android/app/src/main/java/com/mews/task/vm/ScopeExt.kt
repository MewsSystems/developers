package com.mews.task.vm

import toothpick.Scope

inline fun <reified Type> Scope.getDependency(): Type {
    return getInstance(Type::class.java)
}