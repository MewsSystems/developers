import { useFrame } from '@react-three/fiber'
import { MutableRefObject } from 'react'
import { Object3D, Vector2 } from 'three'

type Props = {
  object3D: MutableRefObject<Object3D>
  orbitCenter: Vector2
  orbitRadius: Vector2
  orbitSpeed?: number
  isInverted?: boolean
}

export const useOrbit = ({
  object3D,
  orbitCenter,
  orbitRadius,
  orbitSpeed = 1,
  isInverted = false,
}: Props) => {
  useFrame((state) => {
    const elapsedTime = state.clock.elapsedTime
    const angle = elapsedTime * orbitSpeed
    const sin = Math.sin(angle)
    const cos = Math.cos(angle)

    object3D.current.position.x =
      orbitCenter.x + (isInverted ? cos : sin) * orbitRadius.x
    object3D.current.position.y =
      orbitCenter.y + (isInverted ? sin : cos) * orbitRadius.y
  })
}
