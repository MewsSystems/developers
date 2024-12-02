import { Button, Typography } from "@mui/material";

interface ErrorMessageProps {
  error?: string;
  handleClear: () => void;
}

const ErrorMessage = ({ error, handleClear }: ErrorMessageProps) => (
  <>
    <Typography>
      {error || "Oops! Something went wrong. Please try again later."}
    </Typography>
    <Button onClick={handleClear}>Go to homepage</Button>
  </>
);

export default ErrorMessage;
