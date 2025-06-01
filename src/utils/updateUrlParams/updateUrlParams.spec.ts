import {updateUrlParams} from './updateUrlParams';

describe('updateUrlParams', () => {
  it('should add new parameter when value is provided', () => {
    const params = new URLSearchParams();
    const result = updateUrlParams({params, key: 'page', value: '2'});

    expect(result.get('page')).toBe('2');
  });

  it('should update existing parameter', () => {
    const params = new URLSearchParams('page=1');
    const result = updateUrlParams({params, key: 'page', value: '2'});

    expect(result.get('page')).toBe('2');
  });

  it('should remove parameter when value is null', () => {
    const params = new URLSearchParams('page=2');
    const result = updateUrlParams({params, key: 'page', value: null});

    expect(result.has('page')).toBe(false);
  });

  it('should preserve other parameters', () => {
    const params = new URLSearchParams('page=1&search=movie');
    const result = updateUrlParams({params, key: 'page', value: '2'});

    expect(result.get('page')).toBe('2');
    expect(result.get('search')).toBe('movie');
  });
});
