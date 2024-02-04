import TextField from "@mui/material/TextField";

type MovieSearchInputProps = {
  inputValue: string;
  onChange: (
    event: React.ChangeEvent<HTMLInputElement>
  ) => void;
};

const MovieSearchInput: React.FC<MovieSearchInputProps> = ({
  inputValue,
  onChange,
}) => {
  return (
    <TextField
      value={inputValue}
      onChange={onChange}
      sx={{ m: 1, width: "25ch" }}
      label="Search Movies"
    />
  );
};

export default MovieSearchInput;
