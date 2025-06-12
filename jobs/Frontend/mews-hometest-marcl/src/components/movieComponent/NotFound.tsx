// NotFoundPage.tsx

import React from 'react';
import { Button, Box } from '@mui/material';
import { useNavigate } from 'react-router-dom';

const NotFoundPage: React.FC = () => {
    const navigate = useNavigate();

    const handleBack = () => {
        navigate(-1); // Go back one step in the history
    };

  return (
    <Box
      sx={{
        minHeight: '100vh',
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
      }}
    >
              
      <Box
        sx={{
          backgroundColor: '#fff',
          padding: '20px',
          borderRadius: '8px',
          textAlign: 'center',
          maxWidth: '500px',
          width: '100%',
        }}
      >
        <img src="/404.jpg" alt="Popcorn" style={{ maxWidth: '450px', margin: '0 auto' }} />
        <Button variant="contained" onClick={handleBack} sx={{ marginTop: '20px', display: 'block', margin: '0 auto' }}>
          Go Back
        </Button>
      </Box>
    </Box>
  );
};

export default NotFoundPage;
