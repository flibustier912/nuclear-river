﻿# Сравнение объектов привязки

Алгоритм сравнения объектов привязки - не универсальный.

У позиции номенклатуры есть поле "тип учёта объекта привязки".


Операция сравнения определена для следующих типов:

- Рубрика
- Адрес фирмы
- Рубрика + адреса фирмы
- Рубрика 1 уровня + адрес фирмы

Для остальных типов учёта операция сравнения объектов привязки не определена, сравниваются только номенклатурные позиции по ID.

Если сравниваются две позиции, у которых тип учёта объекта привязки совпадает ("Рубрика" - "Рубрика"), то сравнение тривиально: нужно сравнивать одинаковые части объекта привязки.
Неопределённость начинается, если сравнивать позиции с **разным типом учёта объекта привязки**.

Как сравнить позицию с учётом "Адрес фирмы" с позицией с учётом "Рубрика" ?

Ответ даёт следующая таблица

| Тип учёта объекта привязки | Рубрика | Адрес фирмы | Рубрика + адреса фирмы | Рубрика 1 уровня + адрес фирмы |
|--------------------------------|-----------------------------------------------------|------------------------------------------|---------------------------------------------------|--------------------------------|
| Рубрика | сравнение рубрик | - | - | - |
| Адрес фирмы | x | сравнение адресов | - | - |
| Рубрика + адреса фирмы | сравнение рубрик, адрес отбрасывается | сравнение адресов, рубрика отбрасывается | сравнение рубрик и адресов | - |
| Рубрика 1 уровня + адрес фирмы | **иерархическое** сравнение рубрик, адрес отбрасывается | сравнение адресов, рубрика отбрасывается | **иерархическое** сравнение рубрик, сравнение адресов | сравнение рубрик и адресов |

Чтобы провести иерархическое сравнение рубрик, надо сначала у позиции с рубрикой 3 уровня вычислить рубрику 1 уровня и сравнить её с рубрикой 1 уровня сравниваемой позиции.

# Типы учёта объекта привязки
На самом деле в ERM типов учёта объекта привязки больше, но не все они используются для проверки на сопутствующие\запрещённые позиции.
В проекте river целесообразно провести группировку типов:

| Тип учёта River | Тип учёта ERM |
|---------------------------|--------------------------------------------------------------------|
| Category | CategorySingle, CategoryMultiple, CategoryMultipleAsterix |
| Address | AddressSingle, AddressMultiple |
| AddressCategory | AddressCategorySingle, AddressCategoryMultiple |
| AddressFirstLevelCategory | AddressFirstLevelCategorySingle, AddressFirstLevelCategoryMultiple |
| None | все остальные типы учёта |

Для типа None в River не определена операция сравнения объектов привязки, сравнение производится только по ID позиций номенклатуры.