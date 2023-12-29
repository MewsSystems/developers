'use client'

import React from 'react'

import { Typography } from '@/components'
import { ChartWrapper, TextWrapper } from './ProgressChart.styles'
import { useTheme } from 'styled-components'

type Props = {
  percentage: number
}

export const ProgressChart = ({ percentage }: Props) => {
  const { colors } = useTheme()

  const radius = 39
  const circleLength = radius * 2 * Math.PI
  // Calculate progress length
  const progressLength = (percentage / 100) * circleLength

  return (
    <ChartWrapper>
      <svg width="100%" height="100%" viewBox="0 0 100 100">
        {/* Background track */}
        <circle
          cx="50"
          cy="50"
          r={radius}
          fill="transparent"
          stroke={colors.fill.secondary}
          strokeWidth="7"
        />
        {/* Progress track */}
        <circle
          cx="50"
          cy="50"
          r={radius}
          fill="transparent"
          stroke={colors.fill.primary}
          strokeWidth="7"
          strokeDasharray={circleLength}
          strokeDashoffset={circleLength - progressLength}
          transform="rotate(-90 50 50)" // to start progress from the top
        />
      </svg>
      <TextWrapper>
        <Typography>{`${percentage}%`}</Typography>
      </TextWrapper>
    </ChartWrapper>
  )
}
