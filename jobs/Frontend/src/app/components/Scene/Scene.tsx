'use client'

import { PerspectiveCamera } from '@react-three/drei'
import { SpotLights } from '../SpotLights'

export const Scene = () => (
  <group>
    <mesh>
      <planeGeometry args={[8.5, 4.5]} />
      <meshStandardMaterial roughness={0.55} color="#000000" />
    </mesh>
    <SpotLights />
    <PerspectiveCamera makeDefault fov={25} position={[0, 0, 6]} />
  </group>
)
