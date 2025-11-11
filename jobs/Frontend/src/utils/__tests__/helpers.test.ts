import { SyntheticEvent } from "react"
import { fallbackImageHandler } from "../helpers"

describe('fallbackImageHandler', () => {
  it('should set fallback image props', () => {
    const ev = {
      target: {
        id: '123',
        srcset: 'test'
      }
    } as unknown as SyntheticEvent<HTMLImageElement>

    const actual = fallbackImageHandler(ev)
    expect(actual).toEqual({ target: {
      id: 'image-fallback',
      srcset: 'https://placehold.co/92x138',
    }})
  })
})