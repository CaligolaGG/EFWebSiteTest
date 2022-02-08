import ProductRepo from './Repos/ProductsRepo';
import BrandRepo from './Repos/BrandRepo';
import CategoryRepo from './Repos/CategoryRepo';

const repositories = {
    'products': ProductRepo,
    'brands': BrandRepo, 
    'categories': CategoryRepo,
}
export default {
    get: name => repositories[name]
};