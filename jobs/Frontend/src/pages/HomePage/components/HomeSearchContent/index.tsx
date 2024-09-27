import type { FC } from 'react'
import { HomeSearchContentView } from './HomeSearchContentView'
import { useHomeSearchContent } from '../../hooks/useHomeSearchContent'
import type { UseHomeSearchHook } from '../../types'
import { wrap } from '../../../../utils'

export const HomeSearchContent: FC<UseHomeSearchHook> = wrap(
    HomeSearchContentView,
    useHomeSearchContent,
)
