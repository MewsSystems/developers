import i18n from '../'

jest.unmock('react-i18next')

describe('Test i18n', () => {
  it('Test Instance Creation', () => {
    expect(i18n).toBeDefined()
  })
})
