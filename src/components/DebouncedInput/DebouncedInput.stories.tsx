import type { Meta, StoryObj } from '@storybook/nextjs-vite';
import { useState } from 'react';
import { DebouncedInput } from './DebouncedInput';

const meta: Meta<typeof DebouncedInput> = {
  title: 'Components/DebouncedInput',
  component: DebouncedInput,
  tags: ['autodocs'],
  args: {
    placeholder: 'Type to search...',
    debounceDelay: 500,
    ariaLabel: 'Search input',
  },
};

export default meta;
type Story = StoryObj<typeof DebouncedInput>;

const ControlledWrapper = (args: React.ComponentProps<typeof DebouncedInput>) => {
  const [value, setValue] = useState('');
  return (
    <>
      <DebouncedInput {...args} value={value} onChange={setValue} />
      <p>Debounced Value: {value}</p>
    </>
  );
};

export const Default: Story = {
  render: (args) => <ControlledWrapper {...args} />,
  parameters: {
    docs: {
      source: {
        code: `
const [value, setValue] = useState('');
return (
  <>
    <DebouncedInput
      value={value}
      onChange={setValue}
      debounceDelay={500}
      placeholder="Type to search..."
      ariaLabel="Search input"
    />
    <p>Debounced Value: {value}</p>
  </>
);`,
      },
    },
  },
};

export const WithInitialValue: Story = {
  render: (args) => {
    const [value, setValue] = useState('Initial');
    return (
      <>
        <DebouncedInput {...args} value={value} onChange={setValue} />
        <p>Debounced Value: {value}</p>
      </>
    );
  },
  parameters: {
    docs: {
      source: {
        code: `
const [value, setValue] = useState('Initial');
return (
  <>
    <DebouncedInput
      value={value}
      onChange={setValue}
      debounceDelay={500}
      placeholder="Type to search..."
      ariaLabel="Search input"
    />
    <p>Debounced Value: {value}</p>
  </>
);`,
      },
    },
  },
};

export const CustomDelay: Story = {
  render: (args) => <ControlledWrapper {...args} />,
  args: {
    debounceDelay: 1000,
    placeholder: 'Debounce 1000ms',
  },
  name: 'Custom Debounce Delay',
  parameters: {
    docs: {
      source: {
        code: `
const [value, setValue] = useState('');
return (
  <>
    <DebouncedInput
      value={value}
      onChange={setValue}
      debounceDelay={1000}
      placeholder="Debounce 1000ms"
      ariaLabel="Search input"
    />
    <p>Debounced Value: {value}</p>
  </>
);`,
      },
    },
  },
};

export const WithAriaDescribedBy: Story = {
  render: (args) => {
    const [value, setValue] = useState('');
    return (
      <div>
        <p id="desc">Search the database</p>
        <DebouncedInput {...args} value={value} onChange={setValue} ariaDescribedBy="desc" />
        <p>Debounced Value: {value}</p>
      </div>
    );
  },
  name: 'With aria-describedby',
  parameters: {
    docs: {
      source: {
        code: `
const [value, setValue] = useState('');
return (
  <div>
    <p id="desc">Search the database</p>
    <DebouncedInput
      value={value}
      onChange={setValue}
      debounceDelay={500}
      placeholder="Type to search..."
      ariaLabel="Search input"
      ariaDescribedBy="desc"
    />
    <p>Debounced Value: {value}</p>
  </div>
);`,
      },
    },
  },
};
