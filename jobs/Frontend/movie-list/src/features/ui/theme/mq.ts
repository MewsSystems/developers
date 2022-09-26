export const ScreenSize = {
  small: 576,
  medium: 768,
  large: 1200,
}

export const mq = {
  smallOnly: `@media (max-width: ${ScreenSize.small / 16}em)`,
  small: `@media (min-width: ${ScreenSize.small / 16}em)`,
  medium: `@media (min-width: ${ScreenSize.medium / 16}em)`,
  large: `@media (min-width: ${ScreenSize.large / 16}em)`,
}
