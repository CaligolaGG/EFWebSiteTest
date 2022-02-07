import ProductRepo from './Repos/ProductsRepo';
import BrandRepo from './Repos/BrandRepo';

const repositories = {
    'products': ProductRepo,
    'brands': BrandRepo, 
}
export default {
    get: name => repositories[name]
};