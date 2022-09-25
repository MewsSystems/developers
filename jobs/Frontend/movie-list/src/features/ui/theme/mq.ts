export const ScreenSize = {
  medium: 768,
  large: 1200,
}

export const mq = {
  smallOnly: `@media (max-width: ${ScreenSize.medium / 16}em)`,
  medium: `@media (min-width: ${ScreenSize.medium / 16}em)`,
  large: `@media (min-width: ${ScreenSize.large / 16}em)`,
}
