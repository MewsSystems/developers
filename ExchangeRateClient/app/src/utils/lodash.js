import _ from 'lodash/fp'

import convert from 'lodash/fp/convert'

export const _map = _.map.convert({cap: false})
export const _reduce = _.reduce.convert({cap: false})
export const _filter = _.filter.convert({cap: false})
export const _mapValues = _.mapValues.convert({cap: false})