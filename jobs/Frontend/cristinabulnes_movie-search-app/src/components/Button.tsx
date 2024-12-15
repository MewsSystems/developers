const Button = ({
	children,
	onClick,
}: {
	children: React.ReactNode;
	onClick?: () => void;
}) => {
	return <button onClick={onClick}>{children}</button>;
};

export default Button;
