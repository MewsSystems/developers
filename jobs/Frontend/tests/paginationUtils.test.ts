import { expect, test } from 'vitest';
import { getPaginationModel } from '../src/components/shared/pagination/paginationUtils';

test('getPaginationModel builds valid pagination for page 1', () => {
    expect(getPaginationModel(1, 3)).toEqual([1,2,3]);
    expect(getPaginationModel(1, 5)).toEqual([1,2,3,4,5]);
    expect(getPaginationModel(1, 6)).toEqual([1,2,3,4,5,6]);
    expect(getPaginationModel(1, 7)).toEqual([1,2,3,'...',6,7]);
    expect(getPaginationModel(1, 12)).toEqual([1,2,3,'...',11,12]);
});

test('getPaginationModel builds valid pagination for 6 <= total pages', () => {
    for (let i = 1; i <= 4; i++) {
        expect(getPaginationModel(i, 4)).toEqual([1,2,3,4]);
    }

    for (let i = 1; i <= 6; i++) {
        expect(getPaginationModel(i, 6)).toEqual([1,2,3,4,5,6]);
    }
});

test('getPaginationModel builds valid pagination for 6 > total pages', () => {
    expect(getPaginationModel(2, 22)).toEqual([1,2,3,'...',21,22]);
    expect(getPaginationModel(3, 10)).toEqual([1,2,3,4,'...',10]);
    expect(getPaginationModel(4, 16)).toEqual([1,'...',3,4,5,'...',16]);
    expect(getPaginationModel(5, 100)).toEqual([1,'...',4,5,6,'...',100]);
    expect(getPaginationModel(74, 74)).toEqual([1,2,'...',72,73,74]);
    expect(getPaginationModel(52, 53)).toEqual([1,2,'...',51,52,53]);
    expect(getPaginationModel(45, 47)).toEqual([1,'...',44,45,46,47]);
    expect(getPaginationModel(330, 333)).toEqual([1,'...',329,330,331,'...',333]);
});