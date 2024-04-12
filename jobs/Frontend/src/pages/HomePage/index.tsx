import { FC } from 'react'
import { HomePageView } from './HomePageView'
import { UseHomeHookProps } from './types'
import { wrap } from '../../utils'
import { usePage } from './hooks'

export const HomePage: FC<UseHomeHookProps> = wrap(HomePageView, usePage)
