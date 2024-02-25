import { colors } from '../../styling/colors';

export const styles = {
  cardContainer: {
    textAlign: 'left' as const,
    backgroundColor: colors.darkGray,
    height: '100%',
    display: 'flex',
    flexDirection: 'column' as const,
    justifyContent: 'space-between',
  },
  cardLink: {
    color: colors.whiteSmoke,
    textDecoration: 'underline',
    textDecorationColor: colors.whiteSmoke,
    fontWeight: 'bold',
  },
};
