'use client'

import { useEffect, useRef, useState } from 'react'
import { SpotLight, Vector2 } from 'three'

import { useOrbit } from './orbit.hook'
import { useFrame } from '@react-three/fiber'

const DISTANCE = 15
const INTENSITY = 300
// lower = faster
const INTENSITY_DECREASE_FACTOR = 3

export const SpotLights = () => {
  const topLightRef = useRef<SpotLight>(null!)
  const inverseTopLightRef = useRef<SpotLight>(null!)
  const centerLightRef = useRef<SpotLight>(null!)
  const bottomLightRef = useRef<SpotLight>(null!)

  useOrbit({
    object3D: topLightRef,
    orbitCenter: new Vector2(1, 3),
    orbitRadius: new Vector2(1.5, 0.3),
    orbitSpeed: 1.5,
  })

  useOrbit({
    object3D: inverseTopLightRef,
    orbitCenter: new Vector2(1, 3),
    orbitRadius: new Vector2(1.5, 0.3),
    orbitSpeed: 1.5,
    isInverted: true,
  })

  useOrbit({
    object3D: bottomLightRef,
    orbitCenter: new Vector2(-1, -5),
    orbitRadius: new Vector2(1.5, 1.5),
  })

  const [scrollPosition, setScrollPosition] = useState(0)
  const handleScroll = () => {
    const position = window.pageYOffset
    setScrollPosition(position)
  }

  useEffect(() => {
    window.addEventListener('scroll', handleScroll, { passive: true })

    return () => {
      window.removeEventListener('scroll', handleScroll)
    }
  }, [])

  // decrease intensity of lights as user scrolls
  useFrame(() => {
    const targetIntensity = Math.max(
      INTENSITY - scrollPosition / INTENSITY_DECREASE_FACTOR,
      0,
    )

    ;[topLightRef, inverseTopLightRef, centerLightRef, bottomLightRef].forEach(
      (ref) => {
        ref.current.intensity = targetIntensity
      },
    )
  })

  return (
    <>
      <spotLight
        ref={topLightRef}
        distance={DISTANCE}
        position={[0, 0, 7]}
        intensity={INTENSITY}
        color="#050090"
      />
      <spotLight
        ref={inverseTopLightRef}
        distance={DISTANCE}
        position={[0, 0, 7]}
        intensity={INTENSITY}
        color="#050090"
      />
      <spotLight
        ref={centerLightRef}
        distance={DISTANCE}
        position={[0, -2, 7]}
        intensity={INTENSITY}
        color="#cf005d"
      />
      <spotLight
        ref={bottomLightRef}
        distance={DISTANCE}
        position={[0, 0, 7]}
        intensity={INTENSITY}
        color="#cf005d"
      />
    </>
  )
}
