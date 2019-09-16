import React, { Component, CSSProperties, HTMLAttributes } from 'react';
import Icon, { IconPath } from '@components/ui/Icon';
import IconClose from '@icons/close';

import {
    InputCloseButton,
    InputComponent,
    InputElement,
    InputIcon,
    InputIconContainer,
} from './Input.styled';

export interface InputProps {
    type: string;
    readOnly?: boolean;
    disabled?: boolean;
    className?: string;
    style?: CSSProperties;
    value?: string;
    placeholder?: string;
    autofocus?: boolean;
    onChange?: (newValue: string) => void;
    onBlur?: (newValue: string) => void;
    onIconClick?: (newValue: string) => void;
    onCancel?: (newValue: string) => void;
    onEnterPressed?: Function;
    clearBtn?: boolean;
    icon?: IconPath[];
}

export interface InputState extends HTMLAttributes<HTMLInputElement> {
    value: string
}

export class Input extends Component<InputProps, InputState>{
    constructor(props: InputProps) {
        super(props);

        this.state = {
            value: this.props.value ? this.props.value : ''
        };
    }

    static getDerivedStateFromProps(props: InputProps, state: InputState) {
        return {
            value: props.value ? props.value : state.value
        };
    }

    handleChange = (event: any) => {
        const value = event.target.value;

        this.setState({
            value,
        });

        if (this.props.onChange) {
            this.props.onChange(value);
        }
    }

    handleBlur = (event: any) => {
        const value = event.target.value;

        if (this.props.onBlur) {
            this.props.onBlur(value);
        }
    }

    handleEnterPressed = (event: any) => {
        const { value } = this.state;

        if (event.keyCode == 13 && this.props.onEnterPressed) {
            this.props.onEnterPressed(value);
        }
    }

    handleIconClick = () => {
        const { value } = this.state;

        if (this.props.onIconClick) {
            this.props.onIconClick(value);
        }
    }

    handleOnCancel = () => {
        // we use on cancel when we don't want to search
        // on typing, but only on pressing serch icon
        // in this case we don't use onChange function
        if (this.props.onCancel) {
            this.props.onCancel('');
            // this case is when we update the search results on every change
            // (while typing)
        } else if (this.props.onChange) {
            this.props.onChange('');
        }

        this.setState({ value: '' });
    }

    public render() {
        const {
            type,
            className,
            placeholder,
            style,
            autofocus,
            clearBtn,
            icon,
            onIconClick,
            onChange,
            readOnly,
            disabled
        } = this.props;

        const { value } = this.state;


        /*
        let classes = ClassNames(
            'Input',
            className ? className : '',
            readOnly ? "is-readonly" : '',
            disabled ? "is-disabled" : ''
        );

        classes += icon ? " withIcon" : "";
        classes += onIconClick ? " ActiveIcon" : "";
        */

        return (
            <InputComponent style={style}>
                {icon &&
                    <InputIconContainer
                        onClick={this.handleIconClick}
                    >
                        <InputIcon svgPaths={icon} />
                    </InputIconContainer>
                }
                <InputElement
                    className="InputElement"
                    type={type}
                    value={value}
                    placeholder={placeholder}
                    autoFocus={autofocus}
                    onKeyUp={this.handleEnterPressed}
                    onChange={this.handleChange}
                    readOnly={readOnly || false}
                    disabled={disabled || false}
                    onBlur={this.handleBlur}
                />

                {clearBtn && value !== "" &&
                    <InputCloseButton  onClick={this.handleOnCancel}>
                        <Icon className="Input__Close-btn" svgPaths={IconClose} />
                    </InputCloseButton>
                }
            </InputComponent>
        );
    }
}