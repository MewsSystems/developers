export const styles = {
  container: {
    backgroundRepeat: 'no-repeat',
    backgroundPositionX: 'center',
    backgroundPositionY: 'center',
    backgroundSize: 'cover',
    flex: '1',
    display: 'flex',
    minHeight: '100vh',
    textAlign: 'left' as const,
  },
  container2: {
    backgroundColor: 'rgba(0,0,0,0.8)',
    width: '100%',
    flex: 1,
    display: 'flex',
    justifyContent: 'center',
  },
  container3: {
    flexDirection: 'column' as const,
    display: 'flex',
    justifyContent: 'center',
  },
  container4: {
    display: 'flex',
    justifyContent: 'space-between',
  },
  movieTitle: {
    fontSize: '3.5rem',
    fontWeight: '800',
    flex: 3,
  },
  ratingsContainer: {
    flex: 1,
    flexDirection: 'column' as const,
    display: 'flex',
    justifyContent: 'flex-start',
  },
  infoText: {
    color: '#989898',
    fontWeight: 'bold',
  },
};
