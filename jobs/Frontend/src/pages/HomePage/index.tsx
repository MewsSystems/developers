import type { FC } from 'react'
import { HomePageView } from './HomePageView'
import type { UseHomeHookProps } from './types'
import { wrap } from '../../utils'
import { usePage } from './hooks'

export const HomePage: FC<UseHomeHookProps> = wrap(HomePageView, usePage)
