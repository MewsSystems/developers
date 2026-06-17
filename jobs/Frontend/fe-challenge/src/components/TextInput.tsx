import classNames from 'classnames';

interface TextInputProps {
  value?: string;
  onChange?: (event: React.ChangeEvent<HTMLInputElement>) => void;
  placeholder?: string;
  className?: string;
  id?: string;
  name?: string;
}

const TextInput = ({
  placeholder,
  value,
  onChange,
  className,
  id,
  name,
}: TextInputProps) => {
  return (
    <input
      className={classNames(
        'rounded-default px-4 py-2 border focus:outline-none focus-visible:ring-2 focus-visible:ring-gray-700',
        className,
      )}
      name={name}
      id={id}
      type="text"
      value={value}
      onChange={onChange}
      placeholder={placeholder}
    />
  );
};

export default TextInput;
