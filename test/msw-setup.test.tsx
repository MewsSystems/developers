import { render, screen } from '@testing-library/react';
import { useEffect, useState } from 'react';

function UserComponent() {
  const [name, setName] = useState('');

  useEffect(() => {
    fetch('https://api.example.com/user')
      .then((res) => res.json())
      .then((data) => setName(data.name));
  }, []);

  return <div>{name ? `Hello, ${name}` : 'Loading...'}</div>;
}

test('displays user name from mock API', async () => {
  render(<UserComponent />);
  expect(await screen.findByText(/hello, alice/i)).toBeInTheDocument();
});
